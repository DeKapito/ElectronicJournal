using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal
{
    interface IDataController
    {
        Task<IActionResult> Index();
        IActionResult Create();
        Task<IActionResult> Edit(int? id);
        Task<IActionResult> Delete(int? id);
        Task<IActionResult> DeleteConfirmed(int id);
    }
}
