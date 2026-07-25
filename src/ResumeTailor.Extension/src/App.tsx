import "./App.css";

function App() {
  return (
    <main style={{ padding: "1.5rem" }}>
      <h1>Resume Tailor</h1>
      <p>
        Open a job listing and click generate resume to generated a resume based
        off of your own curated bullets.
      </p>

      <div className="btn-container">
        <button className="btn" type="button">
          Extract Job Details
        </button>
      </div>

      <div className="review-container"></div>
    </main>
  );
}

export default App;
