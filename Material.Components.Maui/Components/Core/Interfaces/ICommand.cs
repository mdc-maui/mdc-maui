using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Material.Components.Maui.Core.Interfaces;
internal interface ICommandElement
{
    ICommand Command { get; set; }
    object CommandParameter { get; set; }
}
