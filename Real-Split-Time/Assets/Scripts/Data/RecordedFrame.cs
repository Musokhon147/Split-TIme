using UnityEngine;

[System.Serializable]
public struct RecordedFrame
{
    public Vector3 position;
    public bool flipX;
    public bool isGrounded;
    public float moveInput;

    public RecordedFrame(Vector3 position, bool flipX, bool isGrounded, float moveInput)
    {
        this.position = position;
        this.flipX = flipX;
        this.isGrounded = isGrounded;
        this.moveInput = moveInput;
    }
}
