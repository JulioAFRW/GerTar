using System;
using System.Collections.Generic;

namespace GERTAR.Modelos;

public partial class TB_USUARIO
{
    public int ID_USUARIO { get; set; }

    public string NM_USUARIO { get; set; } = null!;

    public string PERFIL { get; set; } = null!;
}
