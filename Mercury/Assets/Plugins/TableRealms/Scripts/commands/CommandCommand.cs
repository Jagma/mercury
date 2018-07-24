using UnityEngine;
using System;

namespace tablerealms.comms.message
{

  public class CommandCommand : Command<CommandMessage>
  {

    protected override void Process(CommandMessage message, TableRealmsClientConnection tableRealmsClientConnection)
    {
      if (message.getParams() == null || message.getParams().Length == 0)
      {
        ActionManager.instance.PerformActionAsync(message.command);
      }
      else
      {
        ActionManager.instance.PerformActionAsync(message.command + "[" + message.getParams()[0] + "]");
      }
    }
  }
}

