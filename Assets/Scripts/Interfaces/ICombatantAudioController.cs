using System;

public interface ICombatantAudioController
{
    void PlaySound(CombatantSound sound);
}

public interface ICombatantAudioController<T> : ICombatantAudioController, IAudioController<T> 
    where T : Enum
{ }