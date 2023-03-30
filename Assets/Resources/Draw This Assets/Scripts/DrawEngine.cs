/*
 * 
 * Developed by Olusola Olaoye, 2021
 * 
 * To only be used by those who purchased from the Unity asset store
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrawEngine : MonoBehaviour
{
    public enum DrawTool  // different draw tool types
    {
        Pen,
        Circle,
        Rect,
        Line,
        Eraser,
        Text
    }

    private DrawTool current_tool; // the current draw tool


    public Color chosen_color
    {
        get;
        set;
    }

    public float chosen_size
    {
        get;
        set;
    }

    [SerializeField]
    private GameObject draw_board;

    [SerializeField]
    private FakePixel pixel_prefab;

    [SerializeField]
    private TextTool text_prefab;


    private Stack<GameObject> stack_of_objects = new Stack<GameObject>(); // to store pixels and texts


    private const float standard_eraser_size = 0.1f;


    private bool mouse_has_been_down;



    private Vector3 mouse_drag_start_position; // when we are drawing circles, rects and lines

    private Vector3 mouse_drag_end_position;



    List<FakePixel> preview_buffer = new List<FakePixel>(); // this buffer is where we would store pixels when drawing rects, circles and lines
                                                            // it allows us to be able to preview the drawing of the shapes 



    // this is used filling gaps between pixels when user's mouse moves to fast
    private bool has_assigned_last_pixel;
    private FakePixel first_pixel;
    private FakePixel last_pixel;


    
    private float undo_counter = 0;
    private float undo_every_x_seconds = 0.5f; // how fast we will carry out undo operations (with ctrl + z)


    void Start()
    {
        chosen_color = Color.black; 

        chosen_size = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (current_tool)
        {
            case DrawTool.Pen:

                penUse();

                break;


            case DrawTool.Eraser:

                eraserUse();

                break;


            case DrawTool.Text:

                textUse();

                break;


            case DrawTool.Line:

                trackMouseDragStartAndEndPositions(lineUse);

                break;


            case DrawTool.Rect:

                trackMouseDragStartAndEndPositions(rectUse);

                break;


            case DrawTool.Circle:

                trackMouseDragStartAndEndPositions(circleUse);

                break;


        }


        listenToUndoShortcut();
    }



    private void penUse()
    {
        bool mouse_clicked = Input.GetMouseButtonDown(0);

      

        if ((mouse_clicked || didMouseMove()) && mouseIsNotClickingUI())
        {
            
            FakePixel pixel = Instantiate(pixel_prefab);

            pixel.transform.position = getCanvasSpace();
            
            pixel.transform.localScale *= chosen_size;
            
            stack_of_objects.Push(pixel.gameObject);



            if(has_assigned_last_pixel)
            {
                last_pixel = pixel;
            }
            else
            {
                first_pixel = pixel;
            }


            if(first_pixel && last_pixel)
            {
                drawLineFrom(first_pixel.transform.position, last_pixel.transform.position);
            }
            

            has_assigned_last_pixel = !has_assigned_last_pixel;
        }

        if(Input.GetMouseButtonUp(0))
        {
            first_pixel = last_pixel = null;
        }
    }



    private void eraserUse()
    {
        clearPreviewBuffer();

        if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            // destroy any collider that is near the mouse and that is not the draw board
            foreach (Collider object_ in Physics.OverlapSphere(getCanvasSpace(), standard_eraser_size * chosen_size))
            {
                if(object_.gameObject != draw_board)
                {
                    Destroy(object_.gameObject);
                }
            }
        }
    }

    private void textUse()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            TextTool text = Instantiate(text_prefab);

            text.gameObject.transform.position = getCanvasSpace();
            
            text.text.rectTransform.sizeDelta *= chosen_size;
            
            stack_of_objects.Push(text.gameObject);
        }
    }

    private void trackMouseDragStartAndEndPositions(System.Action<bool> draw_function)
    {
        

        if (Input.GetMouseButton(0) && !mouse_has_been_down && EventSystem.current.currentSelectedGameObject == null)
        {
            mouse_drag_start_position = getCanvasSpace();

            mouse_has_been_down = true;


        }

        if(Input.GetMouseButton(0) && didMouseMove() && mouseIsNotClickingUI())
        {
            clearPreviewBuffer();

            mouse_drag_end_position = getCanvasSpace();

            draw_function?.Invoke(true);

           
        }



        if (Input.GetMouseButtonUp(0) && mouse_has_been_down )
        {
            mouse_drag_end_position = getCanvasSpace();

            draw_function?.Invoke(false);

            mouse_has_been_down = false;
        }
    }


    private void drawLineFrom(Vector3 start, Vector3 end, bool preview = false)
    {

        Vector3 normalised_vector = (end - start).normalized;
        float vector_length = (end - start).magnitude;

        float i = 0;
        while (i < vector_length)
        {
            FakePixel pixel = Instantiate(pixel_prefab);

            pixel.transform.position = start + (normalised_vector * i);

            pixel.transform.localScale *= chosen_size;

            stack_of_objects.Push(pixel.gameObject);

            if(preview)
            {
                preview_buffer.Add(pixel);

            }


            i += 0.1f;
        }
    }

    private void lineUse(bool preview)
    {

       
        drawLineFrom(mouse_drag_start_position, mouse_drag_end_position, preview);
    }

    private void rectUse(bool preview)
    {
        

        //upper horizontal
        drawLineFrom(new Vector3(mouse_drag_start_position.x, mouse_drag_start_position.y, mouse_drag_start_position.z),
                    new Vector3(mouse_drag_end_position.x, mouse_drag_start_position.y, mouse_drag_start_position.z), preview);
        
        //lower horizontal
        drawLineFrom(new Vector3(mouse_drag_start_position.x, mouse_drag_end_position.y, mouse_drag_start_position.z),
                   new Vector3(mouse_drag_end_position.x, mouse_drag_end_position.y, mouse_drag_start_position.z), preview);


        // left vertical
        drawLineFrom(new Vector3(mouse_drag_end_position.x, mouse_drag_start_position.y, mouse_drag_start_position.z),
                    new Vector3(mouse_drag_end_position.x, mouse_drag_end_position.y, mouse_drag_start_position.z), preview);



        // right vertical
        drawLineFrom(new Vector3(mouse_drag_start_position.x, mouse_drag_start_position.y, mouse_drag_start_position.z),
                    new Vector3(mouse_drag_start_position.x, mouse_drag_end_position.y, mouse_drag_start_position.z), preview);

        
    }

    private void circleUse(bool preview)
    {
        
        Vector3 circle_center = (mouse_drag_end_position + mouse_drag_start_position) / 2;

        float radius = (mouse_drag_end_position - mouse_drag_start_position).magnitude / 2;

        float theta = 0;
       
        while (theta < 2 * Mathf.PI) // put fake pixels around the center of the circle
        {
            FakePixel pixel = Instantiate(pixel_prefab);

            pixel.transform.position = circle_center + new Vector3(Mathf.Cos(theta) * radius, Mathf.Sin(theta) * radius, 0);

            pixel.transform.localScale *= chosen_size;

            stack_of_objects.Push(pixel.gameObject);

            // pixel should rotate on its z axis depending on theta
            pixel.transform.rotation = Quaternion.Euler(0,0, theta);

            if(preview)
            {
                preview_buffer.Add(pixel);
            }

            
            theta += ((0.1f / radius) * chosen_size);
        }
    }






    public void setPenTool()
    {
        current_tool = DrawTool.Pen;
    }

    public void setEraserTool()
    {
        current_tool = DrawTool.Eraser;
    }

    public void setTextTool()
    {
        current_tool = DrawTool.Text;
    }


    public void setLineTool()
    {
        current_tool = DrawTool.Line;
    }

    public void setCircleTool()
    {
        current_tool = DrawTool.Circle;
    }

    public void setRectTool()
    {
        current_tool = DrawTool.Rect;
    }


    public void paintBackground()
    {
        Camera.main.backgroundColor = chosen_color;
    }

    public void Undo()
    {
        clearPreviewBuffer();

        // undo 25 at a time
        for (int i = 0; i < 25; i++)
        {
            if(stack_of_objects.Count > 0)
            {
                Destroy(stack_of_objects.Pop());
            }
        }
    }


    private void clearPreviewBuffer()
    {
        // remove all "fake pixels"
        foreach (FakePixel pixel in preview_buffer)
        {
            Destroy(pixel.gameObject);
        }

        preview_buffer.Clear();
    }




    public void clearCanvas()
    {
        clearPreviewBuffer();

        // remove all "fake pixels"
        foreach (FakePixel pixel in FindObjectsOfType<FakePixel>())
        {
            Destroy(pixel.gameObject);
        }

        // remove all texts
        foreach (TextTool text in FindObjectsOfType<TextTool>())
        {
            Destroy(text.gameObject);
        }
    }




    // mouse click ray cast hit to drawing board's collider
    private Vector3 getCanvasSpace()
    {
        
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private bool didMouseMove()
    {
        return (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") != 0) || (Input.GetMouseButton(0) && Input.GetAxis("Mouse Y") != 0);
    }

    private bool mouseIsNotClickingUI()
    {
        return EventSystem.current.currentSelectedGameObject == null;
    }


    private void listenToUndoShortcut()
    {
        if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.Z))
        {
            if (undo_counter < undo_every_x_seconds)
            {
                undo_counter += Time.deltaTime;
            }
            else
            {
                Undo();
                undo_counter = 0;
            }
        }
    }

}
