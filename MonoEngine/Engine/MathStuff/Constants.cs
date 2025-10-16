namespace MonoEngine.Engine.MathStuff;

/// <summary>Ensemble de constante liées à la physique</summary>
public static class Constants
{
    /// <summary>Accéleration de la gravité</summary>
    public const float Gravity = 50f;

    /// <summary>Coefficient  de la friction de l'air</summary>
    public const float AirFriction = 0.1f;

    extension(float)
    {
#pragma warning disable S1244 // Floating point numbers should not be tested for equality
        public static bool IsNull(float f) => f == 0;
#pragma warning restore S1244 // Floating point numbers should not be tested for equality
    }
}