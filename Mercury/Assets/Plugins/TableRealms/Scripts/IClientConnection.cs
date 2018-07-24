using System;

public interface IClientConnection {
    string GetRemoteEndPoint();
    bool IsConnected();
    void Close();

    void Write(string buffer);
    void ReadAndDeliverTo(Func<string,bool> callback);
}
