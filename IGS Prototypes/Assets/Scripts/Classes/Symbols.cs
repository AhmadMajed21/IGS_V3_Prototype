using System.Collections.Generic;

public static class Symbols
{
    public static Dictionary<Power, string> symbols = new Dictionary<Power, string>();

    static Symbols()
    {
        symbols = new()
        {
            {Power.Alpha,"Α"}, {Power.Beta,"Β"}, {Power.Gamma,"Γ"}, {Power.Delta,"Δ"},
            {Power.Epsilon,"Ε"}, {Power.Zeta,"Ζ"}, {Power.Eta,"Η"}, {Power.Theta,"Θ"},
            {Power.Iota,"Ι"}, {Power.Kappa,"Κ"}, {Power.Lambda,"Λ"}, {Power.Mu,"Μ"},
            {Power.Nu,"Ν"}, {Power.Xi,"Ξ"}, {Power.Omicron,"Ο"}, {Power.Pi,"Π"},
            {Power.Rho,"Ρ"}, {Power.Sigma,"Σ"}, {Power.Tau,"Τ"}, {Power.Upsilon,"Υ"},
            {Power.Phi,"Φ"}, {Power.Chi,"Χ"}, {Power.Psi,"Ψ"}, {Power.Omega,"Ω"}
        };
    }

    public static string ToCustomString(this List<Power> power)
    {
        string str = "";

        foreach (Power symbol in power)
        {
            str += symbols[symbol] + " ";
        }

        return str;
    }
}
