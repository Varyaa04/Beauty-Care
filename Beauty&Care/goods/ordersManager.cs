//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Beauty_Care.goods
{
    using System;
    using System.Collections.Generic;
    
    public partial class ordersManager
    {
        public int idOrderManager { get; set; }
        public int idOrder { get; set; }
        public int idGoods { get; set; }
    
        public virtual beautyGoods beautyGoods { get; set; }
        public virtual orders orders { get; set; }
    }
}
