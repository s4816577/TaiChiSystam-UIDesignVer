#include <boost/asio.hpp>
#include <boost/array.hpp>
#include <boost/thread.hpp>
#include <iostream>
#include <opencv2/opencv.hpp>

using boost::asio::ip::udp;

cv::Mat frame;
cv::VideoCapture camera;

void handle_send(const boost::system::error_code& /*error*/,
                 std::size_t /*bytes_transferred*/)
{
}

void send_something(std::string host, int port)
{

    boost::asio::io_service io_service;

    udp::socket socket(io_service);
    socket.open(udp::v4());

    while (true){

        camera >> frame;
        std::vector<unsigned char> buffer;

        cv::resize(frame, frame, cv::Size(100, 100));
        cv::imencode(".jpg", frame, buffer);
        std::cout << buffer.size() << std::endl;

        boost::system::error_code myError;
        socket.async_send_to(boost::asio::buffer(buffer), udp::endpoint(boost::asio::ip::address::from_string(host, myError) , port), boost::bind(&handle_send, boost::asio::placeholders::error, boost::asio::placeholders::bytes_transferred));

        // boost::this_thread::sleep(boost::posix_time::milliseconds(24));
    }

    socket.close();
}

int main()
{
    camera = cv::VideoCapture(0);
    if(!camera.isOpened())
        return -1;

    boost::thread t;
    t = boost::thread(boost::bind(&send_something, "192.168.0.12", 12345));
    t.join();

    return 0;
}
