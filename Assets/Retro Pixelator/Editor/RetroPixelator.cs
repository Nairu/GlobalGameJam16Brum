using System.Threading;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;

class RetroPixelator : EditorWindow {

	private Vector2 scrollPos = Vector2.zero;

	public class ThreadData
	{
		public int start;
		public int end;
		public ThreadData (int s, int e) {
			start = s;
			end = e;
		}
	}
	
	private static Color[] texColors;
	private static Color[] newColors;
	private static int w;
	private static float ratioX;
	private static float ratioY;
	private static int w2;
	private static int finishCount;
	private static Mutex mutex;

	public Texture2D inputTexture;
	private int pixelation=16;
	private int resolution=1;
	private Texture2D outputTexture=null;

	[MenuItem ("Window/Retro Pixelator")]
	public static void  ShowWindow () {
		var window = EditorWindow.GetWindow(typeof(RetroPixelator));
		window.position = new Rect (100, 100, 232, 590);
	}
		
	void OnGUI () {
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true, GUILayout.Width (position.width), GUILayout.Height (position.height));

		EditorGUILayout.LabelField ("Source Image");

		inputTexture = (Texture2D) EditorGUILayout.ObjectField(inputTexture, typeof(Texture2D), GUILayout.MaxWidth(210), GUILayout.MinHeight(210));

		EditorGUILayout.Space();

		EditorGUILayout.LabelField ("Pixelation Setting", GUILayout.MaxWidth(210));
		pixelation = EditorGUILayout.IntSlider(pixelation, 4, 32, GUILayout.MaxWidth(210));
		if (pixelation % 2 != 0) {
			pixelation--;
		}

		EditorGUILayout.Space();

		EditorGUILayout.LabelField ("Output Image", GUILayout.MaxWidth(210));
		GUI.DrawTexture( new Rect(GUILayoutUtility.GetLastRect().x, GUILayoutUtility.GetLastRect().y, 210, 210), outputTexture);
		for (int i=1; i<34; i++) {
			EditorGUILayout.Space ();
		}

		EditorGUILayout.LabelField ("Pixel Resolution", GUILayout.MaxWidth(210));
		resolution = EditorGUILayout.IntSlider(resolution, 1, 32, GUILayout.MaxWidth(210));
		if (resolution % 2 != 0) {
			resolution--;
		}
		if (resolution == 0) {
			resolution=1;
		}

		EditorGUILayout.Space ();

		if(GUILayout.Button( "Pixelate!", GUILayout.MaxWidth(210))) {
			if(inputTexture){
				string path = AssetDatabase.GetAssetPath(inputTexture);
				TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
				textureImporter.isReadable = true; 
				textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				AssetDatabase.ImportAsset(path);
				
				outputTexture = Instantiate(inputTexture);
				if(ThreadedPointScale (outputTexture, pixelation, pixelation, path.Substring(0, path.Length - 4) + "_" + pixelation + "x.png", resolution)){
					//Debug.Log("Finished pixelate!");
					outputTexture = (Texture2D) AssetDatabase.LoadAssetAtPath(path.Substring(0, path.Length - 4) + "_" + pixelation + "x.png", typeof(Texture2D));
					path = AssetDatabase.GetAssetPath(outputTexture);
					textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
					textureImporter.filterMode = FilterMode.Point;
					AssetDatabase.ImportAsset(path);
					
				}
				if(resolution>1){
					path = AssetDatabase.GetAssetPath(outputTexture);
					textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
					textureImporter.isReadable = true; 
					textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
					AssetDatabase.ImportAsset(path);
					
					outputTexture = Instantiate(outputTexture);
					if(ThreadedPointScale (outputTexture, pixelation*resolution, pixelation*resolution, path, resolution)){
						//Debug.Log("Finished resolution!");
						outputTexture = (Texture2D) AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
						path = AssetDatabase.GetAssetPath(outputTexture);
						textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
						textureImporter.filterMode = FilterMode.Point;
						AssetDatabase.ImportAsset(path);
					}
				}
			}
		}
		EditorGUILayout.EndScrollView ();
		EditorGUILayout.Space ();
	}

	private bool ThreadedPointScale (Texture2D tex, int newWidth, int newHeight, string filepath, int pixelres)
	{
		texColors = tex.GetPixels();
		newColors = new Color[newWidth * newHeight];

		ratioX = ((float)tex.width) / newWidth;
		ratioY = ((float)tex.height) / newHeight;

		w = tex.width;
		w2 = newWidth;
		var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
		var slice = newHeight/cores;
		
		finishCount = 0;
		if (mutex == null) {
			mutex = new Mutex(false);
		}
		if (cores > 1)
		{
			int i = 0;
			ThreadData threadData;
			for (i = 0; i < cores-1; i++) {
				threadData = new ThreadData(slice * i, slice * (i + 1));
				ParameterizedThreadStart ts = new ParameterizedThreadStart(PointScale);
				Thread thread = new Thread(ts);
				thread.Start(threadData);
			}
			threadData = new ThreadData(slice*i, newHeight);
			PointScale(threadData);

			while (finishCount < cores)
			{
				Thread.Sleep(1);
			}
		}
		else
		{
			ThreadData threadData = new ThreadData(0, newHeight);
			PointScale(threadData);
		}

		tex.Resize(newWidth, newHeight);
		tex.SetPixels(newColors);
		tex.Apply();

		// Encode texture into PNG
		var bytes = tex.EncodeToPNG();

		File.WriteAllBytes(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + filepath, bytes);
		EditorApplication.ExecuteMenuItem("Assets/Refresh");

		return true;
	}

	private static void PointScale (System.Object obj)
	{
		ThreadData threadData = (ThreadData) obj;
		for (var y = threadData.start; y < threadData.end; y++)
		{
			var thisY = (int)(ratioY * y) * w;
			var yw = y * w2;
			for (var x = 0; x < w2; x++) {
				newColors[yw + x] = texColors[(int)(thisY + ratioX*x)];
			}
		}
		
		mutex.WaitOne();
		finishCount++;
		mutex.ReleaseMutex();
	}
	
	private static Color ColorLerpUnclamped (Color c1, Color c2, float value)
	{
		return new Color (c1.r + (c2.r - c1.r)*value, 
		                  c1.g + (c2.g - c1.g)*value, 
		                  c1.b + (c2.b - c1.b)*value, 
		                  c1.a + (c2.a - c1.a)*value);
	}

}