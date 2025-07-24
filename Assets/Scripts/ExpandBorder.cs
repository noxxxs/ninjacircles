using UnityEngine;

public class ExpandBorder : MonoBehaviour
{
    private enum ExpandDirection
    {
        Up, Down, Left, Right
    }


    [SerializeField] private ExpandDirection _direction;
    [SerializeField] private Transform _border;
    [SerializeField] private Transform _firstPoint;
    [SerializeField] private Transform _secondPoint;
    [Range(0.01f, 1.5f)]
    public float _expandSpeed = 0.5f;
    [SerializeField] private float _width;
    [SerializeField] private bool _showGizmoPoints;

    

    private bool _canExpand;
    private bool _expandFinished = false;
    public bool ExpandFinished
    {
        get { return _expandFinished; }
        set { _expandFinished = value; }
    }

    private void Start()
    {
        //Set up width X
        if (_direction == ExpandDirection.Left || _direction == ExpandDirection.Right)
        {
            _border.localScale = new Vector3(0f, _width, 0f);
        }
        //Set up width Y
        else if (_direction == ExpandDirection.Up || _direction == ExpandDirection.Down)
        {
            _border.localScale = new Vector3(_width, 0f, 0f);
        }

        // Set up border transform
        _border.GetComponent<SpriteRenderer>().enabled = true;
        _border.position = _firstPoint.position;
    }
    void FixedUpdate()
    {
        if (_canExpand)
        Expand(_border, _expandSpeed);
    }

    void Expand(Transform square, float expandAmount)
    {
        // Expand X
        if (_direction == ExpandDirection.Left || _direction == ExpandDirection.Right)
        {
            // Invert expand direction
            int invert = 1;
            if (_direction == ExpandDirection.Left)
                invert = -1;

            square.localScale += new Vector3(expandAmount * invert, 0f, 0f);
            square.position += new Vector3(expandAmount * invert / 2f, 0f, 0f);

            // Check if border reached second point
            if (Vector3.Distance(_border.position + new Vector3 (_border.localScale.x / 2f,0,0), _secondPoint.position) <= _expandSpeed)
            {
                ExpandFinished = true;
                _canExpand = false;
            }
            else
            {
                _canExpand = true;
            }
        } 
        // Expand Y
        else if (_direction == ExpandDirection.Up || _direction == ExpandDirection.Down)
        {
            // Invert expand direction
            int invert = 1;
            if (_direction == ExpandDirection.Down)
                invert = -1;
            square.localScale += new Vector3(0f, expandAmount * invert, 0f);
            square.position += new Vector3(0f, expandAmount * invert / 2f, 0f);

            // Check if border reached second point
            if (Vector2.Distance(_border.position + new Vector3(0, _border.localScale.y / 2f, 0), _secondPoint.position) <= _expandSpeed)
            {
                ExpandFinished = true;
                _canExpand = false;
            }
            else
            {
                _canExpand = true;
            }
        } 
        else
        {
            Debug.Log("Wrong expand direction");
        }
    }

    public void StartExpand()
    {
        _canExpand = true;
    }

    private void OnDrawGizmos()
    {
        if (_showGizmoPoints)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(_firstPoint.position, Vector3.one * _width);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(_secondPoint.position, Vector3.one * _width);
        }
    }
}
