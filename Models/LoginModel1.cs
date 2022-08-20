using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testmongo.Models
{
    public class LoginModel1
    {
        //[Key]
        //[Display(Name = "Tên đăng nhập")]
        //[Required(ErrorMessage = "Bạn phải nhập tài khoản")]
        public string UserName { set; get; }

        //[Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        //[Display(Name = "Mật khẩu")]
        public string Password { set; get; }
    }
}