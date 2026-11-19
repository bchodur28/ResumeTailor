import { Link } from "react-router-dom";

const NotFoundPage = () => {
  return (
    <div className="flex flex-col items-center justify-center gap-4 py-24">
      <h1 className="text-4xl font-bold">404</h1>
      <p className="text-gray-600">Page not found.</p>

      <Link to="/" className="primary-color font-semibold">
        Return to Home
      </Link>
    </div>
  );
};

export default NotFoundPage;
