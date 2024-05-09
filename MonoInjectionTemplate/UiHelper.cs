using System;
using System.Reflection;
using UnityEngine;

namespace MonoInjectionTemplate
{
    public static class UIHelper
    {
        private static float
            x, y,
            width, height,
            margin,
            controlHeight,
            controlDist,
            nextControlY,
            scale;

        private static GUIStyle _styleButton = new GUIStyle(GUI.skin.button);
        
        
        
        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }


        public static void Begin(string text, float _x, float _y, float _width, float _height,
            float _margin, float _controlHeight, float _controlDist, float nextY = 20f, float _scale = 1f)
        {
            scale = _scale;
            x = _scale * _x;
            y = _scale *_y;
            width = _scale *_width;
            height = _scale *_height;
            margin = _scale *_margin;
            controlHeight = _scale *_controlHeight;
            controlDist =  _scale *_controlDist;
            nextControlY =  (y + nextY * _scale);
            _styleButton.fontSize = (int)(14 * _scale);
            
            GUI.Box(new Rect(x, y, width, height), text);
        }
 
        private static Rect NextControlRect()
        {
            Rect r = new Rect(x + margin, nextControlY, width - margin * 2, controlHeight);
            nextControlY +=  (controlHeight + controlDist);
            return r;
        }
 
        public static string MakeEnable(string text, bool state)
        {
            return string.Format("{0}: {1}", text, state ? "ON" : "OFF");
        }
 
        public static bool Button(string text, bool state)
        {
            return Button(MakeEnable(text, state));
        }
 
        public static bool Button(string text)
        {
            return GUI.Button(NextControlRect(), text, _styleButton);
        }
 
        public static void Label(string text, float value, int decimals = 2)
        {
            Label(string.Format("{0}{1}", text, Math.Round(value, 2).ToString()));
        }
 
        public static void Label(string text)
        {
            GUI.Label(NextControlRect(), text);
        }
 
        public static float Slider(float val, float min, float max)
        {
            return GUI.HorizontalSlider(NextControlRect(), val, min, max);
        }
    }
}