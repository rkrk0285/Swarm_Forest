using System;

namespace Domino.Networking.HTTP
{
    public enum HTTPMethod
    {
        GET,
        POST
    }
    public abstract class HTTPRequest
    {
        public string Router { get; protected set; }
        public HTTPMethod Method { get; protected set; }
    }

    [Serializable]
    public class AuthenticationRequest : HTTPRequest
    {
        public string Username { get; protected set; }
        public string Password { get; protected set; }

        private AuthenticationRequest()
        {
            Router = "login";
            Method = HTTPMethod.POST;
        }
        public class Factory
        {
            public static AuthenticationRequest Create(string Username, string Password)
            {
                return new AuthenticationRequest
                {
                    Username = Username,
                    Password = Password
                };
            }
        }
    }

    [Serializable]
    public class AuthenticationResponse
    {
        public int userId { get; set; }
        public string sessionId { get; set; }
        public string matchServerAddress { get; set; }
        public string gameServerAddress { get; set; }
    }

    [Serializable]
    public class RankData
    {
        public int Rank { get; protected set; }
        public string Username { get; protected set; }
        public int Rating { get; protected set; }
    }

    [Serializable]
    public class RankRequest : HTTPRequest
    {
        public int From { get; protected set; }
        public int To { get; protected set; }

        private RankRequest()
        {
            Router = "ranking";
            Method = HTTPMethod.GET;
        }

        public class Factory
        {
            public static RankRequest Create(int From, int To)
            {
                return new RankRequest
                {
                    From = From,
                    To = To
                };
            }
        }
    }

    [Serializable]
    public class RankResponse
    {
        public RankData[] RankDatas { get; protected set; }

        private RankResponse() { }
    }
}