import Card from "../components/ui/Card";

const Generate = () => {
  return (
    <div className="flex justify-center">
      <div>
        <h1 className="text-2xl font-semibold mb-4">Generate your Resume</h1>
        <Card className="w-5xl flex flex-col">
          <textarea className="border border-gray-300 rounded p-2 w-full h-[80vh]"></textarea>
          <button className="btn self-end mt-2">Generate Resume</button>
        </Card>
      </div>
    </div>
  );
};

export default Generate;
