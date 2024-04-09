using Godot;
using System;
using System.Diagnostics;

public partial class CameraFreeLookController : Node3D
{
    [Export]
    public Camera3D Camera;

    [Export] public float Speed = 5f;

    [Export] public float SpeedRunMultiplier = 2f;
    
    [Export] public float MouseSensitivity = 3f;

    private Vector2 Offset = Vector2.Zero;
    
    private bool RotationEnabled;

    private float CurrentSpeed;

    public void Initialize(bool enabled)
    {
        SetProcess(enabled);
        SetProcessInput(enabled);
        SetPhysicsProcess(enabled);
    }

    public override void _Ready()
    {
        CurrentSpeed = Speed;
    }

    private void OnRotationChanged(bool obj)
    {
        if (obj)
        {
            EnableRotation();
        }
        else
        {
            DisableRotation();
        }
    }

    private void DisableRotation()
    {
        var screenCenter = GetViewport().GetVisibleRect().Size / 2;
        
        GetViewport().WarpMouse(screenCenter);
        
        RotationEnabled = false;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    private void EnableRotation()
    {
        RotationEnabled = true;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        RotateMouse(delta);
        
        Offset = Vector2.Zero;
        
        var horizontal = 0;
        var vertical = 0;

        if (Input.IsKeyPressed(Key.A))
        {
            horizontal -= 1;
        }

        if (Input.IsKeyPressed(Key.D))
        {
            horizontal += 1;
        }

        if (Input.IsKeyPressed(Key.W))
        {
            vertical -= 1;
        }

        if (Input.IsKeyPressed(Key.S))
        {
            vertical += 1;
        }
        
        var inputVector = new Vector3(horizontal, 0, vertical);

        if (Input.IsKeyPressed(Key.Space))
        {
            inputVector.Y += 1;
        }

        if (Input.IsKeyPressed(Key.Ctrl))
        {
            inputVector.Y -= 1;
        }

        GlobalPosition += GlobalTransform.Basis * inputVector * (float) delta * CurrentSpeed;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("rotate"))
        {
            EnableRotation();
        }

        if (Input.IsActionJustReleased("rotate"))
        {
            DisableRotation();
        }

        if (!RotationEnabled) return;
        
        if (@event is InputEventMouseMotion mouseMotion)
        {
            Offset = mouseMotion.Relative;
        }
    }

    private void RotateMouse(double delta)
    {
        if (!RotationEnabled) return;
        
        var x = -Offset.X;
        var y = -Offset.Y;
        
        Camera.RotateX(Mathf.DegToRad(y * (float)delta * MouseSensitivity));
        RotateY(Mathf.DegToRad(x * (float)delta * MouseSensitivity));
    }
}
