using JetBrains.Annotations;


namespace MonoInjectionTemplate.callableElements
{
    public class CEButton : CallableElement
    {
        private readonly string _text;
        private bool _isToggle;
        public bool Active;

        [CanBeNull] private CheatFieldBool _cf;
        private delegate int BoolWrap(bool n);

        private BoolWrap v;
        

        public CEButton(string text)
        {
            _text = text;
        }

        public CEButton Set(bool  val)
        {
            _isToggle = true;
            Active = val;
            return this;
        }

        public CEButton Control(CheatFieldBool cf)
        {
            _isToggle = true;
            Active = cf.Value;
            _cf = cf;
            return this;
        }

        public override void Draw()
        {
            
            if (_isToggle && UIHelper.Button(_text, Active))
            {
                Active = !Active;
                _cf?.SetActive(Active);
                Call();
            }
            else if (!_isToggle && UIHelper.Button(_text))
            {
                Call();
            }
        }
    }
}