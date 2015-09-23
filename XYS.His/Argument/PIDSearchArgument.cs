using XYS;
namespace XYS.His.Argument
{
    public sealed class PIDSearchArgument:SearchArgument
    {
        private string _pid;
        public PIDSearchArgument()
            :base("病案号参数",100L)
        {
        }
        public PIDSearchArgument(string pid)
            : base("病案号参数", 100L)
        {
            this._pid = pid;
        }

        public string PID
        {
            get { return this._pid; }
            set { this._pid = value; }
        }
    }
}
