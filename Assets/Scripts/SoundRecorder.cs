using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SoundRecorder : MonoBehaviour
{
    private int bufferSize;
    private int numBuffers;
    private int outputRate = 44100;
    private string fileName = "recTest.wav";
    private int headerSize = 44;
    private bool recOutput;
    private FileStream fileStream;

    public bool currentRecordStatus = false;
    public bool mixStatus = false;
    public bool playStatus = false;

    public string path;

    public AudioSource mixingSpeaker;
    private FileInfo[] info;
    private DirectoryInfo dir;
    public string recordFileName;
    public Text recordText;
    public Text mixText;
    public Text playText;

    int cnt = 0;

    public void GameExit()
    {
        Application.Quit();
    }

    public void RecordButtonTouch()
    {
        if (currentRecordStatus == false)
        {
            recOutput = true;
            currentRecordStatus = true;
            Debug.Log("Record Start");
            StartWriting(fileName);
            recordText.text = "RECORD\nSTOP";
        }
        else
        {
            recOutput = false;
            currentRecordStatus = false;
            Debug.Log("Record End");
            WriteHeader();
            recordText.text = "RECORD\nSTART";
        }
    }

    public void MixButtonTouch()
    {
        if (mixStatus == false)
        {
            recOutput = true;
            mixStatus = true;
            StartWriting(fileName);
            mixText.text = "MIX\nSTOP";
        }
        else
        {
            recOutput = false;
            mixStatus = false;
            WriteHeader();
            mixText.text = "MIX\nSTART";
        }
    }


///////////////////////////////////////////////////////////////////////////

    public void PlayButtonTouch()
    {
        if(playStatus == false)
        {
            StartCoroutine("LoadAndPlaySound");
            playStatus = true;
            playText.text = "PLAY\nSTOP";
        }
        else
        {
            mixingSpeaker.Stop();
            playStatus = false;
            playText.text = "PLAY";
        }
    }

//////////////////////////////////////////////////////////////////////////

      

    IEnumerator LoadAndPlaySound()
    {
        
        string audioPath = "file:///" + recordFileName;

        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.WAV);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("isNetworkError");
        }
        else
        {
            AudioClip audioTrack = DownloadHandlerAudioClip.GetContent(www);
            mixingSpeaker.Stop();
            mixingSpeaker.clip = audioTrack;
            mixingSpeaker.loop = true;
            mixingSpeaker.Play();
        }
    }

    void Start()
    {
        AudioSettings.GetDSPBufferSize(out bufferSize, out numBuffers);
        mixingSpeaker = GameObject.Find("Speaker").GetComponent<AudioSource>();
    }

    public string pathForDocumentsFile(string fileName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string _path = Application.persistentDataPath;
            path = _path.Substring(0, _path.LastIndexOf('/'));
            return Path.Combine(path, fileName);
        }

        return null;
    }

    public void StartWriting(string name)
    {
        recordFileName = pathForDocumentsFile(name);
        fileStream = new FileStream(recordFileName, FileMode.Create, FileAccess.Write);

        byte emptyByte = new byte();

        for (int i = 0; i < headerSize; i++)
        {
            fileStream.WriteByte(emptyByte);
        }
    }



    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (recOutput)
        {
            ConvertAndWrite(data);
        }
    }

    public void ConvertAndWrite(float[] dataSource)
    {
        Int16[] intData = new Int16[dataSource.Length];
        Byte[] bytesData = new Byte[dataSource.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < dataSource.Length; i++)
        {
            intData[i] = (Int16)(dataSource[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        fileStream.Write(bytesData, 0, bytesData.Length);
    }
    public void WriteHeader()
    {
        fileStream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(two);
        fileStream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(outputRate);
        fileStream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(outputRate * 4);
        fileStream.Write(byteRate, 0, 4);

        UInt16 four = 4;
        Byte[] blockAlign = BitConverter.GetBytes(four);
        fileStream.Write(blockAlign, 0, 2);

        UInt16 sixteen = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(sixteen);
        fileStream.Write(bitsPerSample, 0, 2);

        Byte[] dataString = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(dataString, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(fileStream.Length - headerSize);
        fileStream.Write(subChunk2, 0, 4);

        fileStream.Close();
    }
}
