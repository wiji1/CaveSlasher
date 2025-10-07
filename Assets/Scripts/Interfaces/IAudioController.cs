using System;

public interface IAudioController<T> where T : Enum
{
    void PlaySound(T sound);
}