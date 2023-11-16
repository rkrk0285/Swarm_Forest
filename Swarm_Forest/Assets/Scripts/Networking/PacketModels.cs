using System;
using UnityEngine;


///////////////////////////////////////////////
///                                         ///
///               TCP Packets               ///
///                                         ///
///////////////////////////////////////////////

namespace Domino.Networking.TCP
{
    [Serializable]
    public enum PacketID: int{
        Invalid = 0,
        MatchRegister,
        MatchCancel,
        MatchCreated,
        MatchJoin,
        MoveObject,
        CastSkill,
        ObjectDead,
        InstantiateObject,
        UpdateObjectStatus,
        EliteSpawnTimer,

    }

    [Serializable]
    public class PacketBase{
        public PacketID PacketID{get; protected set;}
    }


    [Serializable]
    public class MatchCreated: PacketBase{
        public int RoomID{get; protected set;}

        private MatchCreated(){
            // PacketID = PacketID.MatchCreated;
        }
    }


    [Serializable]
    public class MatchRegister: PacketBase{

        private MatchRegister(){
            PacketID = PacketID.MatchRegister;
        }

        public class Factory{
            public static MatchRegister Create(){
                return new MatchRegister();
            }
        }
    }

    [Serializable]
    public class MatchCancel: PacketBase{

        private MatchCancel(){
            PacketID = PacketID.MatchCancel;
        }

        public class Factory{
            public static MatchCancel Create(){
                return new MatchCancel();
            }
        }
    }

    [Serializable]
    public class MatchJoin: PacketBase{
        public int RoomID{get; protected set;}

        private MatchJoin(){
            PacketID = PacketID.MatchJoin;
        }
        public class Factory{
            public static MatchJoin Create(int RoomID){
                return new MatchJoin{
                    RoomID = RoomID
                };
            }
        }
    }

    [Serializable]
    public class MoveObject: PacketBase{
        public int ObjectID{get; protected set;}
        public Vector3 Position{get; protected set;}

        private MoveObject(){
            PacketID = PacketID.MoveObject;
        }
        public class Factory{
            public static MoveObject Create(int ObjectID, Vector3 Position){
                return new MoveObject{
                    ObjectID = ObjectID,
                    Position = Position
                };
            }
        }
    }

    [Serializable]
    public class CastSkill: PacketBase{
        public int ObjectID{get; protected set;}
        public Vector3 Direction{get; protected set;}
        public int SkillID{get; protected set;}

        private CastSkill(){
            PacketID = PacketID.CastSkill;
        }
        public class Factory{
            public static CastSkill Create(int ObjectID, Vector3 Direction, int SkillID){
                return new CastSkill{
                    ObjectID = ObjectID,
                    Direction = Direction,
                    SkillID = SkillID
                };
            }
        }
    }

    [Serializable]
    public class ObjectDead: PacketBase{
        public int ObjectID{get; protected set;}

        private ObjectDead(){
            PacketID = PacketID.ObjectDead;
        }
        public class Factory{
            public static ObjectDead Create(int ObjectID){
                return new ObjectDead{
                    ObjectID = ObjectID
                };
            }
        }
    }

    [Serializable]
    public class InstantiateObject: PacketBase{
        public int ObjectID{get; protected set;}
        public Vector3 Position{get; protected set;}

        private InstantiateObject(){
            PacketID = PacketID.InstantiateObject;
        }
    }

    [Serializable]
    public class UpdateObjectStatus: PacketBase{
        public int ObjectID{get; protected set;}
        public int Level{get; protected set;}
        public int HP{get; protected set;}

        private UpdateObjectStatus(){
            PacketID = PacketID.UpdateObjectStatus;
        }
    }

    [Serializable]
    public class EliteMonsterTimer: PacketBase{
        public int ObjectID{get; private set;}
        public int Remains{get; private set;}

        private EliteMonsterTimer(){
            PacketID = PacketID.EliteSpawnTimer;
        }
    }
}

///////////////////////////////////////////////
///                                         ///
///               HTTP Packets              ///
///                                         ///
///////////////////////////////////////////////

namespace Domino.Networking.HTTP
{
    public enum HTTPMethod{
        GET,
        POST
    }
    public abstract class HTTPRequest{
        public string Router{get; protected set;}
        public HTTPMethod Method{get; protected set;}
    }

    [Serializable]
    public class AuthenticationRequest: HTTPRequest{
        public string Username{get; protected set;}
        public string Password{get; protected set;}

        private AuthenticationRequest(){
            Router = "login";
            Method = HTTPMethod.POST;
        }
        public class Factory{
            public static AuthenticationRequest Create(string Username, string Password){
                return new AuthenticationRequest{
                    Username = Username,
                    Password = Password
                };
            }
        }
    }

    [Serializable]
    public class AuthenticationResponse{
        public string UserID{get;protected set;}
        public string SessionID{get;protected set;}

        private AuthenticationResponse(){}
    }

    [Serializable]
    public class RankData{
        public int Rank{get;protected set;}
        public string Username{get;protected set;}
        public int Rating{get;protected set;}
    }

    [Serializable]
    public class RankRequest: HTTPRequest{
        public int From{get;protected set;}
        public int To{get;protected set;}

        private RankRequest(){
            Router = "ranking";
            Method = HTTPMethod.GET;
        }

        public class Factory{
            public static RankRequest Create(int From, int To){
                return new RankRequest{
                    From = From,
                    To = To
                };
            }
        }
    }

    [Serializable]
    public class RankResponse{
        public RankData[] RankDatas{get; protected set;}

        private RankResponse(){}
    }
}