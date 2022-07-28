from pythonosc import udp_client


class OSC():
    '''
    OSCのクライアントクラス
    '''
    def __init__(self, ip='127.0.0.1', port=5005) -> None:
        self.client = udp_client.SimpleUDPClient(ip, port)

    def send_body_points(self, points:list, keypoints: list):
        '''
        OSCサーバーに姿勢頂点を送信
        '''
        for i in range(len(points)):
            address = '/body/' + keypoints[i]
            self.client.send_message(address, points[i])
