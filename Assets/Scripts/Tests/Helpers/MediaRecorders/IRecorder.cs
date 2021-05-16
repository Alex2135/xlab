using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPlayer
{
    void ReadyToPlay();
    void PlayRecord();
}

public interface IRecorder
{
    event Action<string> OnRecordSave;
    void ReadyToRecord(object obj);
    void Record(object obj);
    void PlayRecord(IPlayer _player);
    void DeleteRecord(object obj);
    void SendRecord(object obj);
}