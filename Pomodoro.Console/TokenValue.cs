namespace Endowdly.Pomodoro.Console
{
    internal class TokenValue
    {
        public readonly Token ParentToken;

        public TokenValue(Token token)
        {
            ParentToken = token;
        }

        private bool Equals(TokenValue other)
        {
            return ParentToken.Equals(other.ParentToken);
        }

        public override bool Equals(object o)
        {
            return (o is TokenValue)
                ? Equals((TokenValue)o)
                : false;
        }

        public override int GetHashCode()
        {
            return ParentToken.GetHashCode() | 0x10001000;   // mask with xor to get original token
        }
    }
}
