using UnityEngine;

public class MoveCameraFollow : MonoBehaviour
{
    public float panningSpeed = 35f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    public float maxYlim;
    public float minYlim;
    public float scrollSpeed = 20f;
    public float smoothSpeed = 0.125f;
    public float minY = 20;
    public float maxY = 120;
    public Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            panningSpeed = 60f;
            scrollSpeed = 35f;
        }
        else
        {
            panningSpeed = 35f;
            scrollSpeed = 20f;
        }

        Vector3 pos = transform.position;
        if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panningSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panningSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panningSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panningSpeed *  Time.deltaTime;
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -minYlim, maxYlim);

        transform.position = pos;

    }
}
