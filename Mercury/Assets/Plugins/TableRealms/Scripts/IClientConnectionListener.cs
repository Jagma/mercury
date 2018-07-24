using System;

public interface IClientConnectionListener {
    void Initialize(string gameName);
    string GetConnectionString();
    void AcceptClientConnections(Func<IClientConnection, bool> callback);
}
