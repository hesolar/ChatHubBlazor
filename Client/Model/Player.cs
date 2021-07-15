using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//si no hace falta la borramos

namespace BlazorApp.Client.Model {
    public class Player {
        [Required]
        [StringLength(10,ErrorMessage = "Name is too long.")]
        public string Name { get; set; }
        public string CurrentRoom { get; set; }

}
}
