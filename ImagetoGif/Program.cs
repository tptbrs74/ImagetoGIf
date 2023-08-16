using System;
using ImageMagick;

class Program
{
    static void Main(string[] args)
    {
        string projectDirectory = Environment.CurrentDirectory;
        //Please change 
        string playgroundImagePath = Path.Combine(projectDirectory, "Playground.png");
        string footballImagePath = Path.Combine(projectDirectory, "Football.png");

        Console.WriteLine("Enter the number of total frames:");
        int totalFrames = GetValidIntegerInput();

        Console.WriteLine("Enter the pixel movement of the football in each frame:");
        int yMovementPerFrame = GetValidIntegerInput();

        Console.WriteLine("Enter the delay between each frame (in milliseconds):");
        int delayBetweenFrames = GetValidIntegerInput();

        Console.WriteLine("Enter the initial X co-ordinate:");
        int initialX = GetValidIntegerInput();

        Console.WriteLine("Enter the initial Y co-ordinate:");
        int initialY = GetValidIntegerInput();

        //Output File will be created at C:\Users\tptbr\source\repos\ImagetoGif\ImagetoGif\bin\Debug\net6.0\OutputFile.gif
        string outputGifPath = Path.Combine(projectDirectory, "OutputFile.gif");

        CreateBouncingFootballGif(playgroundImagePath, footballImagePath, outputGifPath,
            totalFrames, yMovementPerFrame, delayBetweenFrames, initialX, initialY);
    }

    static void CreateBouncingFootballGif(string playgroundImagePath, string footballImagePath,
        string outputGifPath, int totalFrames, int yMovementPerFrame, int delayBetweenFrames,
        int initialX, int initialY)
    {
        using (MagickImageCollection collection = new MagickImageCollection())
        {
            using (MagickImage playgroundImage = new MagickImage(playgroundImagePath))
            using (MagickImage footballImage = new MagickImage(footballImagePath))
            {
                for (int i = 0; i < totalFrames; i++)
                {
                    MagickImage frame = new MagickImage(playgroundImage.Clone());

                    int newY = initialY - i * yMovementPerFrame;
                    frame.Composite(footballImage, initialX, newY, CompositeOperator.Over);

                    collection.Add(frame);
                }
            }

            // Set the GIF settings
            collection[0].AnimationDelay = delayBetweenFrames;
            for (int i = 1; i < totalFrames; i++)
            {
                collection[i].AnimationDelay = delayBetweenFrames;
                collection[i].AnimationDelay += collection[i - 1].AnimationDelay;
            }

            // Save the GIF
            collection.Write(outputGifPath, MagickFormat.Gif);
        }

        Console.WriteLine("GIF created successfully.");
    }

    static int GetValidIntegerInput()
    {
        int value;
        while (!int.TryParse(Console.ReadLine(), out value) || value <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer:");
        }
        return value;
    }
}
