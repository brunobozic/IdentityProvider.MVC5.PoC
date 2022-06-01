namespace Module.CrossCutting.Email
{
    public class IncomingMailMessageDto
    {
        public string Destination { get; internal set; }
        public string Subject { get; internal set; }
        public string Body { get; internal set; }
    }
}