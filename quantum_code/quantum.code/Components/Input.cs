using Photon.Deterministic;

namespace Quantum
{
    public partial struct Input
    {
        private static FPVector2 anchorVector = FPVector2.Right;

        public FPVector2 Direction
        {
            get
            {
                if(EncodedDirection == default)
                {
                    return FPVector2.Zero;
                }

                // Do lúc encode cộng thêm 1 thì giờ phải trừ đi 1
                var angle = (EncodedDirection - 1) * 2;
                var angleRad = FP.Deg2Rad * angle;

                return new FPVector2(FPMath.Cos(angleRad), FPMath.Sin(angleRad));
            }

            set
            {

                if(value == default)
                {
                    EncodedDirection = default;
                    return;
                }

                var directionAngle = FPVector2.RadiansSigned(anchorVector, value) * FP.Rad2Deg;

                // Cộng thêm 1 do số 0 được dùng để biểu thị cho giá trị không di chuyển
                directionAngle = (((directionAngle + 360) % 360) / 2) + 1;
                EncodedDirection = (byte)directionAngle.AsInt;
            }
        }
    }
}
