namespace Module.CrossCutting.MessagePattern
{
    public interface IBaseResponse
    {
        bool Success { get; set; }

        //public bool Success
        //{
        //	get { return _success; }
        //	set { _success = value; }
        //}
        string ErrorMessage { get; set; }

        //public string ErrorMessage
        //{
        //	get { return _errorMessage; }
        //	set { _errorMessage = value; }
        //}
        string StatusMessage { get; set; }
        //public string StatusMessage
        //{
        //	get { return _statusMessage; }
        //	set { _statusMessage = value; }
        //}

        string AuthenticationToken { get; set; }
    }
}