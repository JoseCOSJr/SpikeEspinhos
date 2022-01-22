using UnityEngine;

public class cursoInvisible : MonoBehaviour
{
    private float cursoDontUseTime = 0f;
    private Vector2 oldCursoPos = Vector2.zero;
   
    // Update is called once per frame
    void Update()
    {
        Vector2 newPosCurso = Input.mousePosition;
        if(oldCursoPos  == newPosCurso && !(Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)))
        {
            if(cursoDontUseTime > 5f)
            {
                Cursor.visible = false;
            }
            else
            {
                cursoDontUseTime += Time.unscaledDeltaTime;
            }
        }
        else
        {
            Cursor.visible = true;
            cursoDontUseTime = 0f;
            oldCursoPos = newPosCurso;
        }
    }
}
