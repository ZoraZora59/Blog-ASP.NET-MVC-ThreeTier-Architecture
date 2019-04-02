using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogRefactored.Models
{
    public class UserViewModel
    {
        private UserViewModel model;

        public UserViewModel(UserViewModel model)
        {
            this.model = model;
        }

        [Key]
        [StringLength(maximumLength: 16, ErrorMessage = "用户名必须在16位以内")]
        [RegularExpression("^\\w+$", ErrorMessage = "用户名必须是由数字、字母、下划线的组合")]
        public string Account { set; get; }

        [Required]
        [StringLength(maximumLength: 16, ErrorMessage = "昵称必须在16位以内")]
        public string Name { set; get; }

        [Required]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = "密码必须在6~16位数之间")]
        [RegularExpression("[\\u4e00-\\u9fa5]", ErrorMessage = "密码别用中文哦")]
        public string Password { set; get; }

        [Required]
        [StringLength(maximumLength: 16, MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        public string Cpassword { set; get; }

        [Required]
        [StringLength(4, ErrorMessage = "注意是4个数字的验证码哦")]
        public string Code { set; get; }
    }
}