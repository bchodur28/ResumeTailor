type InputProps = {
  id: string;
  label: string;
  type?: string;
  required?: boolean;
};

const Input = ({ id, label, type = "text", required = false }: InputProps) => {
  return (
    <div className="flex flex-col">
      <label className="text-lg" htmlFor={id}>
        {label}
      </label>
      <input
        className="border p-2 rounded border-gray-300"
        type={type}
        id={id}
        required={required}
      />
    </div>
  );
};

export default Input;
