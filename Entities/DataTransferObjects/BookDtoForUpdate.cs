using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    //public record BookDtoForUpdate
    //{
    //    public int Id { get; init; }
    //    public string Title { get; init; }
    //    public int Price { get; init; }
    //}


    // farkli yazilisi olarak
    //public record BookDtoForUpdate(int id, string Title, decimal Price);

    public record BookDtoForUpdate : BookDtoForManipulation
    {
        [Required]
        public int Id { get; init; }
    };


}
