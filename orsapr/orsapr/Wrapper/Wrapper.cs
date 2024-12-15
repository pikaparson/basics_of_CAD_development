using System;
using System.Collections.Generic;
using KompasAPI7;
using Kompas6Constants;
using Kompas6Constants3D;

namespace orsapr.Wrapper
{
    public class Wrapper
    {
        struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public void Build(Parameters parameters)
        {
            // Open cad
            Type t = Type.GetTypeFromProgID("KOMPAS.Application.7");
            IKompasAPIObject kompas7 = (IKompasAPIObject)Activator.CreateInstance(t);
            kompas7.Application.Visible = true;

            // Create part
            kompas7.Application.Documents.Add(DocumentTypeEnum.ksDocumentPart);
            IKompasDocument3D document3d = (IKompasDocument3D)kompas7.Application.ActiveDocument;
            IPart7 part = document3d.TopPart;
            IModelContainer modelContainer = (IModelContainer)part;

            // Create seat
            ISketch seatSketch = CreateSketch(modelContainer, part, "Эскиз: Сидушка");
            DrawRectangle(seatSketch, 0, 0, parameters.SeatWidth, parameters.SeatLength);
            CreateExtrusion(modelContainer.Extrusions, seatSketch, parameters.SeatThickness, "Элемент выдавливания: Сидушка", false);

            // Create legs
            List<Point> coords = new List<Point>()
            {
                new Point(0, 0),
                new Point(parameters.SeatWidth - parameters.LegWidth, 0),
                new Point(0, parameters.SeatLength - parameters.LegWidth),
                new Point(parameters.SeatWidth - parameters.LegWidth, parameters.SeatLength - parameters.LegWidth)
            };

            int legNumber = 0;
            foreach (var point in coords)
            {
                legNumber++;
                ISketch legSketch = CreateSketch(modelContainer, part, "Эскиз: Ножка " + legNumber);
                DrawRectangle(legSketch, point.X, point.Y, parameters.LegWidth, parameters.LegWidth);
                CreateExtrusion(modelContainer.Extrusions, legSketch, -parameters.LegLength, "Элемент выдавливания: Ножка " + legNumber, false);
            }
        }

        private ISketch CreateSketch(IModelContainer modelContainer, IPart7 part, string name)
        {
            ISketch sketch = modelContainer.Sketchs.Add();
            sketch.Plane = part.DefaultObject[ksObj3dTypeEnum.o3d_planeXOY];
            sketch.Name = name;
            sketch.Hidden = false;
            sketch.Update();

            return sketch;
        }

        private void DrawRectangle(ISketch sketch, int x, int y, int width, int height)
        {
            IKompasDocument documentSketch = sketch.BeginEdit();
            IKompasDocument2D document2D = (IKompasDocument2D)documentSketch;
            IViewsAndLayersManager viewsAndLayersManager = document2D.ViewsAndLayersManager;
            IView view = viewsAndLayersManager.Views.ActiveView;
            IDrawingContainer drawingContainer = (IDrawingContainer)view;

            IRectangle rectangle = drawingContainer.Rectangles.Add();
            rectangle.Style = (int)Kompas6Constants.ksCurveStyleEnum.ksCSNormal;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            rectangle.Update();
            sketch.EndEdit();
        }

        private void CreateExtrusion(IExtrusions extrusions, ISketch sketch, double depth, string name, bool draftOutward)
        {
            IExtrusion extrusion = extrusions.Add(Kompas6Constants3D.ksObj3dTypeEnum.o3d_bossExtrusion);
            extrusion.Direction = Kompas6Constants3D.ksDirectionTypeEnum.dtNormal;
            extrusion.Name = name;
            extrusion.Hidden = false;
            extrusion.ExtrusionType[true] = Kompas6Constants3D.ksEndTypeEnum.etBlind;
            extrusion.DraftOutward[true] = draftOutward;
            extrusion.DraftValue[true] = 0.0;
            extrusion.Depth[true] = depth;

            IExtrusion1 extrusion1 = (IExtrusion1)extrusion;
            extrusion1.Profile = sketch;
            extrusion1.DirectionObject = sketch;
            extrusion.Update();
        }
    }
}