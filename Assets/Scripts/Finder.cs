using UnityEngine;
using OpenCvSharp;
using OpenCvSharp.Demo;

public class Finder : WebCamera
{
    [SerializeField] private FlipMode ImageFlip;
    [SerializeField] private float Treshold = 96f;
    [SerializeField] private bool ShowProcessedImage = true;
    [SerializeField] private float CurveAccuracy = 10f;
    [SerializeField] private float MinArea = 5000f;
    [SerializeField] private PolygonCollider2D PolygonCollider;

    private Mat image;
    private Mat processedImage = new Mat();
    private Point[][] contours;
    private HierarchyIndex[] hierarchy;
    private Vector2[] vectorList;

    // protected override void Awake()
    // {
    //     if (Surface == null)
    //         Surface = gameObject;

    //     base.Awake();
    // }

    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        image = OpenCvSharp.Unity.TextureToMat(input);

        Cv2.Flip(image, image, ImageFlip);
        Cv2.CvtColor(image, processedImage, ColorConversionCodes.BGR2GRAY);
        Cv2.Threshold(processedImage, processedImage, Treshold, 255, ThresholdTypes.BinaryInv);
        Cv2.FindContours(processedImage, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

        PolygonCollider.pathCount = 0;
        foreach(Point[] contour in contours)
        {
            Point[] approx = Cv2.ApproxPolyDP(contour, CurveAccuracy, true);
            var area = Cv2.ContourArea(contour);

            if (area > MinArea)
            {
                drawContours(processedImage, new Scalar(127, 127, 127), 2, approx);

                PolygonCollider.pathCount++;
                PolygonCollider.SetPath(PolygonCollider.pathCount-1, toVector2(approx));
            }
        }

        if (output == null)
            output = OpenCvSharp.Unity.MatToTexture(ShowProcessedImage ? processedImage : image);
        else
            OpenCvSharp.Unity.MatToTexture(ShowProcessedImage ? processedImage : image, output);

        return true;
    }

    private Vector2[] toVector2(Point[] points)
    {
        vectorList = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            vectorList[i] = new Vector2(points[i].X, points[i].Y);
        }
        return vectorList;
    }

    private void drawContours(Mat Image, Scalar color, int thickness, Point[] Points)
    {
        for (int i = 1; i < Points.Length; i++)
        {
            Cv2.Line(Image, Points[i-1], Points[i], color, thickness);
        }
        Cv2.Line(Image, Points[Points.Length - 1], Points[0], color, thickness);
    }
}
