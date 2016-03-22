?using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

// Songleから楽曲データを取得する
public class SongleAPI : MonoBehaviour {
	public bool debug;
	public string videourl = ""; //www.nicovideo.jp/watch/smxxxxx

	string songleapi = "https://widget.songle.jp/api/v1/";

	public SongDataJson songdata;
	public BeatJson beatdata;
	public ChordJson chorddata;
	public ChorusJson chorusdata;
	public MelodyJson melodydata;

	public bool downloadCompleated  =false ;

	// Use this for initialization
	void Start () {
		StartCoroutine(Download());
	}

	// Update is called once per frame
	void Update () {


	}

	IEnumerator Download()
	{
		// 歌データの解析
		WWW song = new WWW(songleapi + "song.json?url=" + videourl);
		yield return song;
		songdata = JsonMapper.ToObject<SongDataJson>(song.text);
		if(debug)
		{
			Debug.Log("url " + songdata.url);
			Debug.Log("title " + songdata.title);
			Debug.Log("artist.name " + songdata.artist.name);
			Debug.Log("updateAt " + songdata.updatedAt);
		}

		// 拍データの解析
		WWW beat = new WWW(songleapi + "song/beat.json?url=" + videourl);
		yield return beat;
		beatdata = JsonMapper.ToObject<BeatJson>(beat.text);

		// 和音データの解析
		WWW chord = new WWW(songleapi + "song/chord.json?url=" + videourl);
		yield return chord;
		chorddata = JsonMapper.ToObject<ChordJson>(chord.text);

		// メロディ
		WWW melody = new WWW(songleapi + "song/melody.json?url=" + videourl);
		yield return melody;
		melodydata = JsonMapper.ToObject<MelodyJson>(melody.text);

		// サビ
		WWW chorus = new WWW(songleapi + "song/chorus.json?url=" + videourl);
		yield return chorus;
		chorusdata = JsonMapper.ToObject<ChorusJson>(chorus.text);

		downloadCompleated = true;
		Debug.Log("download compleated");
	}

}


[System.Serializable]
public class SongDataJson
{
	public string url; // songle URL
	public int id; // songle ID
	public string title; // タイトル
	public string permalink; // 配信元URL
	public double duration; // 長さ millsecond
	public double rmsAmplitude; // 振幅
	public string code;
	public Artist artist; // アーティスト情報
	public string createAt;
	public string updatedAt;
	public string recognizedAt;

	// アーティスト情報
	public class Artist
	{
		public int id; // sonoge ID
		public string name; // アーティスト名
	}
}

public class BeatJson
{
	public List<Beat> beats = new List<Beat>();
	public List<Bar> bars = new List<Bar>();
	public class Beat
	{
		public int index;
		public int start;
		public int position;
	}


	public class Bar
	{
		public int start;
		public List<Beat> b_beatdata = new List<Beat>();
		public int index;
	}
}

public class ChordJson
{
	public List<Chord> chords = new List<Chord>();
	public class Chord
	{
		public int index;
		public int start;
		public int duration;
		public string name;
	}
}

public class MelodyJson
{
	public List<Melody> melodys = new List<Melody>();
	public class Melody{
		public int start;
		public int duration;
		public float pitch;
		public int number;
		public int index;
	}
}

public class ChorusJson
{
	public List<segment> chorusSegments = new List<segment>();
	public List<segment> repeatSegments = new List<segment>();

	public class segment
	{
		public int start;
		public int duration;
		public List<repeat> repeats = new List<repeat>();
	}
	public class repeat
	{
		public int start;
		public int duration;
		public int index;
	}

}
