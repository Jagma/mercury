using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Kajabity.Tools.Java;

public class TokenizerFunctions : TokenizerSource {
	
	private string lastPropertiesString;
	private JavaProperties properties=new JavaProperties();
	
	public string GetKey(){
		return "!";
	}
	
	public string ProcessToken(string token,List<TokenizerSource> additionalSources=null){
		string[] strings=token.Split('|');
		if ("p"==strings[0]){
			return ReadProperty(strings);
		} else if ("jd"==strings[0]){
			return JavaLongTSToCSharpDateTime(strings);
		} else if ("jdr"==strings[0]){
			return JavaLongTSToCSharpDateTimeRange(strings);
		} else if ("waited"==strings[0]){
			return WaitedTime(strings);
		} else if ("future"==strings[0]){
			return FutureTime(strings);
		} else if ("past"==strings[0]){
			return PastTime(strings);
		} else if ("pos"==strings[0]){
			return ToPosition(strings);
		} else if ("nice"==strings[0]) {
			return GetNiceName(strings);
		} else if ("listmerge"==strings[0]) {
			
		} else if ("+"==strings[0]) {
			
		} else if (">"==strings[0]) {
			return IsGreaterThan (strings);
		}
		
		return "[Function Error '"+strings[0]+"']";
	}
	
	public string ToPosition(string[] strings){
		if (strings.Length==2){
			int pos;
			if (int.TryParse(strings[1],out pos)){
				switch (pos){
				case 1:
					return "1st";
				case 2:
					return "2nd";
				case 3:
					return "3rd";
				default:
					if (pos>3){
						return ""+pos+"th";
					}
					break;
				}
			}
		}else{
			return "[Wrong #arg 'pos']";
		}
		return "";
	}
	
	public string ReadProperty(string[] strings){
		if (strings.Length==3){
			string key=strings[1];
			if (strings[2]!=lastPropertiesString){
				lastPropertiesString=strings[2];
				Stream propertiesStream = new MemoryStream(Encoding.UTF8.GetBytes(lastPropertiesString));
				properties.Clear();
				properties.Load(propertiesStream);
				propertiesStream.Close();
			}
			return properties.GetProperty(key);
		}else{
			return "[Wrong #arg 'p']";
		}
		
	}
	
	public string JavaLongTSToCSharpDateTime(string[] strings) {
		if (strings.Length==3){
			long javaTimestamp;
			if (long.TryParse(strings[2],out javaTimestamp)){
				string format=strings[1];
				
				TimeSpan ss = TimeSpan.FromMilliseconds(javaTimestamp);
				DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				DateTime ddd = Jan1st1970.Add(ss);
				DateTime final = ddd.ToUniversalTime();
				final=TimeZone.CurrentTimeZone.ToLocalTime(final);
				return final.ToString(format);
			}
			return "[Wrong #arg 'jd||?'] "+strings[2];
			
		}else{
			return "[Wrong #arg 'jd']";
		}
	}
	
	public string JavaLongTSToCSharpDateTimeRange(string[] strings) {
		if (strings.Length==3){
			long fromTimestamp;
			long toTimestamp;
			if (long.TryParse(strings[1],out fromTimestamp) && long.TryParse(strings[2],out toTimestamp)){
				
				TimeSpan ss = TimeSpan.FromMilliseconds(fromTimestamp);
				DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				DateTime ddd = Jan1st1970.Add(ss);
				DateTime from = ddd.ToUniversalTime();
				from=TimeZone.CurrentTimeZone.ToLocalTime(from);
				
				ss = TimeSpan.FromMilliseconds(toTimestamp);
				Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				ddd = Jan1st1970.Add(ss);
				DateTime to = ddd.ToUniversalTime();
				to=TimeZone.CurrentTimeZone.ToLocalTime(to);
				
				if (from.DayOfYear==to.DayOfYear && from.Year==to.Year){
					if (from.Year==System.DateTime.Now.Year){
						return from.ToString("d MMMM Htt")+" - "+to.ToString("Htt");
					}else{
						return from.ToString("d MMMM yyyy Htt")+" - "+to.ToString("Htt");
					}
				}
				if (from.Hour==0 && to.Hour==0){
					if (from.Year==System.DateTime.Now.Year){
						return from.ToString("d MMMM")+" - "+to.ToString("d MMMM");
					}else{
						return from.ToString("d MMMM yyyy")+" - "+to.ToString("d MMMM");
					}
				}
				if (from.Year==System.DateTime.Now.Year){
					return from.ToString("d MMMM Htt")+" - "+to.ToString("d MMMM Htt");
				}else{
					return from.ToString("d MMMM yyyy Htt")+" - "+to.ToString("d MMMM Htt");
				}
			}else{
				return "[Wrong #arg 'jd||?'] "+strings[1]+"|"+strings[2];
			}
		}else{
			return "[Wrong #arg 'jd']";
		}
	}
	
	public string WaitedTime(string[] strings) {
		if (strings.Length==2){
			
			float fromTime;
			if (float.TryParse(strings[1],out fromTime)){
				int seconds=(int)(Time.time-fromTime);
				if (seconds<0){
					seconds=-seconds;
				}
				
				if (seconds<60){
					return String.Format("{0}s",seconds);
				}else if (seconds<3600){
					//return ""+(seconds/60)+"m "+(seconds%60)+"s";
					return String.Format("{1}:{0:00}",seconds%60,seconds/60);
				}else {
					//return ""+(seconds/3600)+"h "+(seconds/60)+"m "+(seconds%60)+"s";
					return String.Format("{2}:{1:00}:{0:00}",seconds%60,(seconds/60)%60,seconds/3600);
				}
			} else{
				return "";
			}
		}else{
			return "[Wrong #arg 'waited']";
		}
	}
	
	public string FutureTime(string[] strings) {
		if (strings.Length==2){
			
			float compareTime;
			//Debug.LogError(String.Format("{0}|{1}",strings[0],strings[1]));
			if (float.TryParse(strings[1],out compareTime)){
				return (compareTime>Time.time).ToString();
			}
		}else{
			return "[Wrong #arg 'future']";
		}
		return false.ToString();
	}
	
	public string PastTime(string[] strings) {
		if (strings.Length==2){
			
			float compareTime;
			if (float.TryParse(strings[1],out compareTime)){
				return (compareTime<Time.time).ToString();
			}
		}else{
			return "[Wrong #arg 'past']";
		}
		return false.ToString();
	}
	
	public string GetTokenChangeKeyFromToken(string token){
		return null;
	}
	
	public string GetNiceName (string[] strings) {
		if (strings.Length == 2) {
			Color color = Color.white;
			int r = (int)(color.r * 255f);
			int g = (int)(color.g * 255f);
			int b = (int)(color.b * 255f);
			int a = (int)(color.a * 255f);
			string hexColor = "<color=#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2") + a.ToString("X2") + ">";
			
			string displayName = strings[1];
			if (strings[1].ToLower().Contains("(dev)") || strings[1].ToLower().Contains("(ai)")) {
				string[] nameParts = strings[1].Split("(".ToCharArray());
				displayName = nameParts[0]+"<color=#ffa500ff>("+nameParts[1]+"</color>";
			}
			displayName = hexColor + displayName + "</color>";
			return displayName;
		}
		
		return null;
	}
	
	public string IsGreaterThan (string[] strings) {
		int part1 = 1, part2 = 1;
		int.TryParse(strings[1], out part1);
		int.TryParse(strings[2], out part2);
		Debug.Log("IS " + strings[1] + part1 + " > " + strings[2] + part2);
		if (part1 > part2) {
			return false.ToString();
		}
		
		return true.ToString();
	}
	
	public string MergeLists () {
		return "";
	}
}
