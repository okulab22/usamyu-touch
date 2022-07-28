import sys
import os
import numpy as np

sys.path.append(os.path.join(os.path.dirname(__file__), './depthai_blazepose'))
from BlazeposeDepthaiEdge import BlazeposeDepthai
from BlazeposeRenderer import BlazeposeRenderer


KEYPOINT_DICT = {
    "nose": 0,
    "left_eye_inner": 1,
    "left_eye": 2,
    "left_eye_outer": 3,
    "right_eye_inner": 4,
    "right_eye": 5,
    "right_eye_outer": 6,
    "left_ear": 7,
    "right_ear": 8,
    "mouth_left": 9,
    "mouth_right": 10,
    "left_shoulder": 11,
    "right_shoulder": 12,
    "left_elbow": 13,
    "right_elbow": 14,
    "left_wrist": 15,
    "right_wrist": 16,
    "left_pinky": 17,
    "right_pinky": 18,
    "left_index": 19,
    "right_index": 20,
    "left_thumb": 21,
    "right_thumb": 22,
    "left_hip": 23,
    "right_hip": 24,
    "left_knee": 25,
    "right_knee": 26,
    "left_ankle": 27,
    "right_ankle": 28,
    "left_heel": 29,
    "right_heel": 30,
    "left_foot_index": 31,
    "right_foot_index": 32
}


class Tracker():
    '''
    姿勢追跡のクラス
    '''
    def __init__(self) -> None:
        self.tracker = BlazeposeDepthai(lm_model='lite',
                                        xyz=True,
                                        stats=False,
                                        internal_frame_height=480,
                                        trace=False)
        self.renderer = BlazeposeRenderer(self.tracker)

    def get_frame(self):
        '''
        カメラのフレームと姿勢頂点を取得
        '''
        frame, body = self.tracker.next_frame()
        return frame, body

    def get_points(self, body):
        '''
        3Dの頂点情報を取得
        '''
        if body is None:
            return None

        points = body.landmarks_world

        if body.xyz_ref:
            translation = body.xyz / 1000
            translation[1] = -translation[1]

            if body.xyz_ref == "mid_hips":
                points = points + translation
            elif body.xyz_ref == "mid_shoulders":
                mid_hips_to_mid_shoulders = np.mean([
                    points[KEYPOINT_DICT['right_shoulder']],
                    points[KEYPOINT_DICT['left_shoulder']]],
                    axis=0)
                points = points + translation - mid_hips_to_mid_shoulders

            # y軸反転
            points = points * np.array([1, -1, 1])

            return points

        else:
            return None

    def draw(self, frame, body):
        '''
        姿勢描画
        '''
        frame = self.renderer.draw(frame, body)
        key = self.renderer.waitKey(delay=1)
        if key == 27 or key == ord('q'):
            return True
        else:
            return False

    def exit(self):
        '''
        終了処理
        '''
        self.renderer.exit()
        self.tracker.exit()
