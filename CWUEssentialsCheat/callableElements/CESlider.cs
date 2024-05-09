using System;
using JetBrains.Annotations;
using Unity.VisualScripting;

namespace MonoInjectionTemplate.callableElements
{
    public class StubElement : CallableElement
    {
        public override void Draw() {}
    }

    public class CESlider : CallableElement
    {
        private readonly string _text;

        [CanBeNull] private CheatFieldFloat _cf;
        private delegate int BoolWrap(bool n);

        private BoolWrap v;
        private float _fromValue;
        private float _toValue;

        public CESlider(float fromValue, float toValue, CheatFieldFloat cf)
        {
            _fromValue = fromValue;
            _toValue = toValue;
            _cf = cf;
        }

        public override void Draw()
        {
            if (_cf.Name != string.Empty)
            {
                UIHelper.Label(_cf.Name + ": ", _cf.Value);
            }
            var newValue = UIHelper.Slider(_cf.Value, _fromValue, _toValue);
            
            if (newValue != _cf.Value)
            {
                _cf.Value = newValue;
                if (_cf.AsInt)
                {
                    _cf.Value = (int) newValue;

                }
                Call();
            }
            
        }
    }
}