const Generate = () => {
  return (
    <div>
      <div className="flex gap-6">
        <div className="w-full">
          <h3 className="primary-color text-xl">Job Description</h3>
          <textarea className="border border-gray-300 rounded p-2 w-full h-[80vh]"></textarea>
        </div>
        <div>
          <h3 className="primary-color text-xl">AI Summary</h3>
          <p>
            Lorem ipsum dolor, sit amet consectetur adipisicing elit. Voluptate
            vitae natus eos fugit ipsam libero ducimus, magni vero aperiam, in
            voluptatibus a earum cumque quam, odit nemo quasi nisi ratione.
          </p>
        </div>
      </div>
    </div>
  );
};

export default Generate;
