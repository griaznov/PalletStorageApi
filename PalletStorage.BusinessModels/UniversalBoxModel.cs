﻿namespace PalletStorage.BusinessModels;

public class UniversalBoxModel
{
    // auto properties
    public virtual double Height { get; }
    public virtual double Width { get; }
    public virtual double Length { get; }
    public virtual double Weight { get; }
    public virtual double Volume { get; }

    public UniversalBoxModel(double width, double length, double height, double weight)
    {
        // Verifying parameters
        if (!IsValidBoxParams(width, length, height) || !IsValidWeight(weight))
        {
            var errorMessage = "You need to enter the following required parameters: width, length, height, weight!";
            throw new ArgumentOutOfRangeException(errorMessage);
        }

        Height = height;
        Width = width;
        Length = length;
        Weight = weight;
        Volume = height * width * length;
    }

    public UniversalBoxModel(double width, double length, double height)
    {
        // Verifying parameters
        if (!IsValidBoxParams(width, length, height))
        {
            var errorMessage = "You need to enter the following required parameters: width, length, height";
            throw new ArgumentOutOfRangeException(errorMessage);
        }

        Height = height;
        Width = width;
        Length = length;
        Volume = height * width * length;
    }

    public static bool IsValidBoxParams(double width, double length, double height)
    {
        return !(width <= 0) && !(length <= 0) && !(height <= 0);
    }

    public static bool IsValidWeight(double weight)
    {
        return !(weight <= 0);
    }
}
