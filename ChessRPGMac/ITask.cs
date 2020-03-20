using System;
namespace ChessRPGMac
{
    public delegate void TaskFinishedHandler(ITask task);

    public interface ITask
    {
        void StartTask();
        event TaskFinishedHandler TaskFinished;
    }
}
