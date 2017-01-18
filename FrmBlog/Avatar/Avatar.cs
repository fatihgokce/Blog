using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrmBlog.Avatar
{
    
    public class Avatar
    {
        List<string> _avatarList;
        public Avatar() { 
        _avatarList=new List<string>{
        "http://www.gravatar.com/avatar/836a1d27c997a194a84536cec1f9780a?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/04b84967d828551db995e592be3fab8b?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/009e26d1ccbbe56cc1b67d6e5605e14e?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/431ac89edb33194bd68430de7a36cec4?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/d8acf5864c149fb7266dbb9565fa8ddd?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/55c35f51ebd5c849efc406b791de9736?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/8ff405a77ca04d3075a8bf095c81d5fe?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/283e795e4abf71ae9e0ab6d954c98431?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/82e075edaac8749c2575f42a9dd10ca6?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/a4ff7ab65f052b14b73af4a8036ee552?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/f85a1202c62cc7e72fa5571f9d43efb4?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/0d70baccd85471a4bbbc13a75880cd47?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/33e21edba46e72be00e4cfc3901ffd80?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/a3a05b1c7dedf93a2ec3b97003aa7959?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/44f6722ade761e1d57da2130d7a5382d?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/45abf60287cd8fd2641d81efbc26165a?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/4f7f4c8facfa9abaa7483c41adab4407?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/b724c3d55ebd27e6ce2e9df65d748ebb?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/12a2d737384003198d8ee9c3a3c5d289?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/728017128299b81a9d84d3468d405630?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/55c35f51ebd5c849efc406b791de9736?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/356ab8ced7308b62b522cb7da345bfb4?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/ee1726e9890b894f3f2b573d9354ffbb?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/9508890804811964149304a4a8694ec1?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/a40384c5bf41a2a3bb05a02ff6c401b4?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/52e2697ba3a09999bc7b7559efd6754d?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/61d2bc0830d4865a62c42c5e23a8c85f?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/9e0ee0f108976614c126721d4c16d58f?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/57679130315b547d3c9ec542acbc5607?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/d8a7eea06d9bb16b3b168afc414a866d?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/5f1dc98ddd586f6972fc1d3f2b8c13ec?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/d9b58929efaf8354e768828a644ee0c2?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/4b41796320a1924e6dba563473c42f7d?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/c1e7a0628425aa2de05bfba4ac9a94a2?s=32&d=identicon&r=PG",
        "http://www.gravatar.com/avatar/c1c612497b2401f0bcd63f85d514d90c?s=32&d=identicon&r=PG"
        };
        }
        public List<string> AvatarList
        {
            get 
            { 
                return _avatarList;
            }
            private set { _avatarList = value; }
        }
        public string GetRandomAvatar()
        {
            Random rasgele = new Random();
            return _avatarList[rasgele.Next(0, _avatarList.Count-1)];
        }
       
    }
}