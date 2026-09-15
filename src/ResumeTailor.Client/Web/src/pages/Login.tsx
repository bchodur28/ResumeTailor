import { useAuth0 } from "@auth0/auth0-react";
import { Navigate } from "react-router-dom";

const Login = () => {
  const { isLoading, isAuthenticated, loginWithRedirect, error } = useAuth0();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="flex flex-col gap-4">
        <h1 className="text-3xl font-bold">ResumeTailor</h1>

        {error && <p className="text-red-500">Error: {error.message}</p>}

        <button
          type="button"
          className="btn"
          onClick={() =>
            loginWithRedirect({
              authorizationParams: {
                prompt: "login",
              },
            })
          }
        >
          Login
        </button>

        <button
          className="btn"
          onClick={() =>
            loginWithRedirect({
              authorizationParams: {
                screen_hint: "signup",
              },
            })
          }
        >
          Sign Up
        </button>
        <p className="text-sm text-gray-500">
          {import.meta.env.VITE_AUTH0_AUDIENCE}
        </p>
      </div>
    </div>
  );
};

export default Login;
