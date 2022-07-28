from src.tracker import Tracker, KEYPOINT_DICT
from src.osc import OSC


def main():
    tracker = Tracker()
    osc = OSC()

    while True:
        frame, body = tracker.get_frame()
        if frame is None:
            break

        if tracker.draw(frame, body):
            break

        points = tracker.get_points(body)

        if not points is None:
            osc.send_body_points(points, list(KEYPOINT_DICT.keys()))

    tracker.exit()


if __name__ == '__main__':
    main()
