using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class TableRealmsModel : MonoBehaviour, TokenizerDataStore {
    public static TableRealmsModel instance = null;

    public Dictionary<string, object> globalData = new Dictionary<string, object>();
    public Dictionary<string, Type> globalDataType = new Dictionary<string, Type>();
    public HashSet<string> globalKeys = new HashSet<string>();

    public void Awake() {
        if (instance == null) {
            instance = this;
            Tokenizer.AddTokenizerSource(this);
            GameObject.DontDestroyOnLoad(gameObject);
        } else {
            GameObject.DestroyImmediate(gameObject);
        }
    }

    public string GetKey() {
        return "T";
    }

    public HashSet<string> GetGlobalDataKeys() {
        return globalKeys;
    }

    public bool IsKeyGlobal(string key) {
        return globalKeys.Contains(key);
    }

    public Type GetDataType(string key) {
        if (globalDataType.ContainsKey(key)) {
            return globalDataType[key];
        }
        return null;
    }

    public void AddGlobalKey(string key) {
        if (!globalKeys.Contains(key)) {
            globalKeys.Add(key);
        }
    }

    public string ProcessToken(string token, List<TokenizerSource> additionalSources = null) {
        object tokenValue = GetData<object>(token, additionalSources);
        if (tokenValue != null) {
            return tokenValue.ToString();
        }
        return "";
    }

    public string GetTokenChangeKeyFromToken(string token) {
        return token;
    }

    public object GetDataRaw(string key, List<TokenizerSource> additionalSources = null) {
        if (globalData.ContainsKey(key)) {
            return globalData[key];
        }
        return null;
    }

    public oftype GetData<oftype>(string key, List<TokenizerSource> additionalSources = null) {
        object currentObject = null;

        if (globalData.ContainsKey(key)) {
            currentObject = globalData[key];
        }
        if (typeof(oftype) == typeof(Color)) {
            if (currentObject == null) {
                currentObject = 255l;
            }
            if (currentObject.GetType() == typeof(double)) {
                currentObject = (long)((double)currentObject);
            }

            int colorNumber = (int)((long)currentObject);
            Color32 cc = new Color32((byte)((colorNumber >> 24) & 255), (byte)((colorNumber >> 16) & 255), (byte)((colorNumber >> 8) & 255), (byte)(colorNumber & 255));
            currentObject = (Color)cc;
            //currentObject = new Color(((float)(colorNumber >> 24) / 255f), ((float)(colorNumber >> 16) / 255f), ((float)((colorNumber & 0xff00) >> 8)) / 255f, ((float)(colorNumber & 0xff) / 255f));
        }
        if (typeof(oftype) == typeof(float) && currentObject == null) {
            currentObject = 0f;
        }
        if (typeof(oftype) == typeof(float) && currentObject.GetType() == typeof(double)) {
            currentObject = (float)(double)currentObject;
        }
        if (typeof(oftype) == typeof(int) && currentObject == null) {
            currentObject = 0;
        }
        if (typeof(oftype) == typeof(int) && currentObject.GetType() == typeof(double)) {
            currentObject = (int)(double)currentObject;
        }
        if (typeof(oftype) == typeof(long) && currentObject == null) {
            currentObject = 0L;
        }
        if (typeof(oftype) == typeof(long) && currentObject.GetType() == typeof(double)) {
            currentObject = (long)(double)currentObject;
        }
        if (typeof(oftype) == typeof(int) && currentObject.GetType() == typeof(double)) {
            currentObject = (int)(double)currentObject;
        }
        if (typeof(oftype) == typeof(int) && currentObject.GetType() == typeof(long)) {
            currentObject = (int)(long)currentObject;
        }
        if (typeof(oftype) == typeof(bool) && currentObject == null) {
            currentObject = false;
        }

        try {
            if (currentObject == null) {
                return default(oftype);
            }
            return (oftype)currentObject;
        } catch (InvalidCastException e) {
            Debug.LogError(String.Format("Failed casting from {0}'{1}' to {2} for key '{3}'", MonoUtils.GetFriendlyName(currentObject.GetType()), currentObject, MonoUtils.GetFriendlyName(typeof(oftype)), key));
            Debug.LogError(e);
            return default(oftype);
        }

    }

    public void SetDataLocalOnly(string key, object value) {
        SetDataLocalOnly(key, value, false);
    }

    public void SetDataLocalOnly(string key, object value, bool forceNew) {
        object oldvalue = null;
        if (globalData.ContainsKey(key)) {
            oldvalue = globalData[key];
        }

        if (globalData.ContainsKey(key)) {
            globalData.Remove(key);
        }
        if (value != null) {
            globalData.Add(key, value);
        }

        if (globalDataType.ContainsKey(key)) {
            if (value != null) {
                if (value.GetType() != globalDataType[key]) {
                    Debug.LogWarning("TableRealms: Changes storage type for '" + key + "' from '" + globalDataType[key] + "' to '" + value.GetType() + "'");
                    globalDataType.Remove(key);
                    globalDataType.Add(key, value.GetType());
                }
            } else {
                globalDataType.Remove(key);
            }
        } else if (value != null) {
            globalDataType.Add(key, value.GetType());
        }

        if (oldvalue != value || forceNew) {
            Tokenizer.DataChanged(GetKey(), key, value);
        }

    }

    public void SetData(string key, object value) {
        SetData(key, value, false);
    }

    public void SetData(string key, object value, bool forceNew) {
        object oldvalue = null;

	    if (value.GetType() == typeof(Color) || value.GetType() == typeof(Color32)) {
            Color32 color = (Color)value;
            value = (long)((((int)(color.r)) << 24) + (((int)(color.g)) << 16) + (((int)(color.b)) << 8) + ((int)(color.a)));
        }

        if (!globalData.ContainsKey(key) && (key.IndexOf(".") == -1 || !TableRealmsGameNetwork.instance.IsClientConnected(key.Substring(0, key.IndexOf("."))))) {
	        //globalData.Add(key, value);
            globalKeys.Add(key);
        }

        SetDataLocalOnly(key, value);

        if ((oldvalue != value || forceNew) && (TableRealmsGameNetwork.host)) {
            TableRealmsGameNetwork.instance.SendData(key, value);
        }

        if (key.Contains(".page")) {
            SetData(key.Replace(".page", ".lastpage"), value, true);
        }
    }
}