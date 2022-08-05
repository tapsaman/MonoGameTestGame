namespace MonoGameTestGame
{
    public class Dialog
    {
        public IDialogContent[] Content;

        public Dialog(params IDialogContent[] content)
        {
            Content = content;
        }

        public Dialog(params string[] messages)
        {
            Content = new IDialogContent[messages.Length];

            for (int i = 0; i < Content.Length; i++)
            {
                Content[i] = new DialogText(messages[i]);
            }
        }

        public static DialogText Text(string message)
        {
            return new DialogText(message);
        }
        public static DialogAsk Ask(string question, params string[] options)
        {
            return new DialogAsk(question, options);
        }
    }

    public class DialogText : IDialogContent
    {
        public string Message;

        public DialogText(string message)
        {
            Message = message;
        }
    }

    public class DialogAsk : IDialogContent
    {
        public string Question;
        public string[] Options;

        public DialogAsk(string question, params string[] options)
        {
            Question = question;
            Options = options;
        }
    }
}