using UnityEngine;

public static class AudioPlayer
{

    public static void PlaySoundWithRandomPitch(AudioClip clip, float volume = 1f)
    {
        PlayClipAtPoint(clip, Camera.main.transform.position, volume, Random.Range(0.9f, 1.1f));
    }

    public static void PlaySound(AudioClip clip, float volume = 1f)
    {
        PlayClipAtPoint(clip, Camera.main.transform.position, volume, 1);
    }

    private static GameObject PlayClipAtPoint(AudioClip clip, Vector3 position, float volume, float pitch)
    {
        GameObject obj = new GameObject();
        obj.transform.position = position;
        AudioSource audio = obj.AddComponent<AudioSource>();
        audio.pitch = pitch;
        audio.PlayOneShot(clip, volume);
        GameObject.Destroy(obj, clip.length / pitch);
        return obj;
    }
}
